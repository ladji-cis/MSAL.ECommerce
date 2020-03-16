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
