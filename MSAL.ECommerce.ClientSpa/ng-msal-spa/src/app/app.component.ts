import { Component, OnInit, OnDestroy } from "@angular/core";
import { Subscription } from "rxjs";
import { BroadcastService, MsalService } from "@azure/msal-angular";
import { ProductService } from "./product/product.service";
import {
  AuthError,
  AuthResponse,
  AuthenticationParameters,
  Account
} from "msal";
import { Router } from "@angular/router";

import { create } from "pkce";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"]
})
export class AppComponent implements OnInit, OnDestroy {
  title = "ng-msal-spa";

  private subscriptions: Subscription[] = [];
  private loggedIn: boolean;
  public isIframe: boolean;
  public userAccount: Account;

  constructor(
    private broadcastService: BroadcastService,
    private authService: MsalService,
    private proudctService: ProductService,
    private router: Router
  ) {
    //  This is to avoid reload during acquireTokenSilent() because of hidden iframe
    this.isIframe = window !== window.parent && !window.opener;
    this.userAccount = this.authService.getAccount();

    if (this.userAccount) {
      this.loggedIn = true;
    } else {
      this.loggedIn = false;
    }
  }

  ngOnInit() {
    this.subscriptions.push(
      this.broadcastService.subscribe("msal:loginSuccess", payload => {
        console.log("login success " + JSON.stringify(payload));
        localStorage.setItem("access_token", payload.accessToken);
        this.loggedIn = true;
        this.userAccount = this.authService.getAccount();
      })
    );
    this.subscriptions.push(
      this.broadcastService.subscribe("msal:loginFailure", payload => {
        console.log("login failure " + JSON.stringify(payload));
        this.loggedIn = false;
        this.userAccount = null;
      })
    );
    this.authService.handleRedirectCallback(
      (redirectError: AuthError, redirectResponse: AuthResponse) => {
        if (redirectError) {
          console.error("Redirect error: ", redirectError);
          return;
        }
        console.log("Redirect success: ", redirectResponse);
      }
    );
  }

  login() {
    const isIE =
      window.navigator.userAgent.indexOf("MSIE ") > -1 ||
      window.navigator.userAgent.indexOf("Trident/") > -1;

    const requestObj: AuthenticationParameters = {
      scopes: [
        "user.read",
        "user.read.all",
        "https://lacisorg.onmicrosoft.com/EcommerceApi/myscope"
      ]
    };

    if (isIE) {
      this.authService.loginRedirect(requestObj);
    } else {
      this.authService.loginPopup(requestObj);
    }
  }
  /**
   * {
    code_verifier: 'u1ta-MQ0e7TcpHjgz33M2DcBnOQu~aMGxuiZt0QMD1C',
    code_challenge: 'CUZX5qE8Wvye6kS_SasIsa8MMxacJftmWdsIA_iKp3I'
}
   */

  private createPkce(): any {
    // https://github.com/bukalapak/pkce-npm

    // let challenge = rand();
    // let verif = hash256(challenge);

    /*
    // {
    //   codeVerifier: 'yzbnPbepnvPl6SUcsBzEf21geEkrzseCDLWAS0uliwKQlDEInT23zV6I2NidkkW4BeF4iVlt6.hdLlCNctqHAPCX7DOYB_7347w1Bk3xmBG10R~Se3~GDTRJfYPUf9.P',
    //   codeChallenge: 'DDSuq_32Mlv86ucLNbNspsJ1QUZYz7dYf6L1AnN9Adk'
    // } */

    return {
      codeChallenge: "codeChallenge",
      codeVerifier: "codeVerifier"
    };
  }

  loginPkce() {
    const pkceCodePair = create();
    localStorage.setItem("pkce", JSON.stringify(pkceCodePair));

    const url = `
      https://login.microsoftonline.com/a6c5d2d9-0864-4a8e-a168-edb2f8c5ca81/oauth2/v2.0/authorize?
      client_id=5a13dd91-a849-488b-8088-c837f5e736d7
      &response_type=code
      &redirect_uri=http://localhost:4200/pkce
      &response_mode=query
      &scope=openid offline_access https://graph.microsoft.com/user.read
      &state=12345
      &code_challenge_method=S256
      &code_challenge=${pkceCodePair.codeChallenge}
    `;
    console.log(url);
    location.href = url;
  }

  logout() {
    this.authService.logout();
    // this.router.navigate(["/"]);
  }

  ngOnDestroy(): void {
    // this.broadcastLoginSuccessSubscription.unsubscribe();
    // this.broadcastLoginFailureSubscription.unsubscribe();
    this.broadcastService.getMSALSubject().next(1);
    if (this.subscriptions) {
      this.subscriptions.forEach(sub => sub.unsubscribe());
    }
  }
}
