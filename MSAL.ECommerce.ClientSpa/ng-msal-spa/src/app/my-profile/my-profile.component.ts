import { Component, OnInit, OnDestroy } from "@angular/core";
import { BroadcastService, MsalService } from "@azure/msal-angular";
import { ProductService } from "../product/product.service";
import { Subscription } from "rxjs";
import {
  AuthError,
  AuthResponse,
  Account,
  AuthenticationParameters
} from "msal";
import { HttpClient } from "@angular/common/http";

@Component({
  selector: "app-my-profile",
  templateUrl: "./my-profile.component.html",
  styleUrls: ["./my-profile.component.css"]
})
export class MyProfileComponent implements OnInit, OnDestroy {
  private users: any[];
  private userAccount: Account;
  private subscriptions: Subscription[] = [];

  constructor(private authService: MsalService, private http: HttpClient) {
    this.userAccount = this.authService.getAccount();
  }

  ngOnInit() {
    this.subscriptions.push(
      this.http
        .get("https://graph.microsoft.com/v1.0/users")
        .subscribe((data: any) => {
          this.users = data.value;
          console.log(this.users);
        })
    );
  }

  ngOnDestroy(): void {
    if (this.subscriptions) {
      this.subscriptions.forEach(sub => sub.unsubscribe());
    }
  }
}
