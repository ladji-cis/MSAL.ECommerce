import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, ParamMap } from "@angular/router";
import { HttpClient } from "@angular/common/http";

@Component({
  selector: "app-pkce",
  templateUrl: "./pkce.component.html",
  styleUrls: ["./pkce.component.css"]
})
export class PkceComponent implements OnInit {
  constructor(private route: ActivatedRoute, private http: HttpClient) {}

  private code: string;
  private state: string;
  private sessionState: string;

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.code = params.code;
      this.state = params.state;
      this.sessionState = params.session_state;
      console.log("CODE:", this.code);
      console.log("STATE:", this.state);
      console.log("SESSION_STATE:", this.sessionState);

      const pkce: any = JSON.parse(localStorage.getItem("pkce"));

      const url =
        "https://login.microsoftonline.com/a6c5d2d9-0864-4a8e-a168-edb2f8c5ca81/oauth2/v2.0/token";
      const headers = { "Content-Type": "application/x-www-form-urlencoded" };
      const formData = new FormData();
      formData.append("Content-Type", "application/x-www-form-urlencoded");
      formData.append("scope", "https://graph.microsoft.com/user.read");
      formData.append("code", this.code);
      formData.append("redirect_url", "http://localhost:4200/pkce");
      formData.append("gran_type", "authorization_code");
      formData.append("client_secret", "vA@STdocFyE5wt_FKlkzqlxct.fH.379");
      formData.append("code_verifier", pkce.codeVerifier);

      this.http
        .post<any>(url, formData, { headers })
        .subscribe(data => {
          console.log(data);
          /*
          {
              "access_token": "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6Ik5HVEZ2ZEstZnl0aEV1Q...",
              "token_type": "Bearer",
              "expires_in": 3599,
              "scope": "https%3A%2F%2Fgraph.microsoft.com%2Fmail.read",
              "refresh_token": "AwABAAAAvPM1KaPlrEqdFSBzjqfTGAMxZGUTdM0t4B4...",
              "id_token": "eyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0.eyJhdWQiOiIyZDRkMTFhMi1mODE0LTQ2YTctOD...",
          }
           */
          // Store ACCESS_TOKEN in LocalStorage.
        });
    });
  }
}
