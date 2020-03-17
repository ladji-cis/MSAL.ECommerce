import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";

import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";

import { MsalModule, MsalInterceptor } from "@azure/msal-angular";
import { MyProfileComponent } from "./my-profile/my-profile.component";
import { ProductComponent } from "./product/product.component";
import { ProductService } from "./product/product.service";
import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { ProductDetailComponent } from "./product/product-detail.component";
import { Logger, LogLevel } from "msal";
import { PkceComponent } from "./pkce/pkce.component";

export const protectedResourceMap: [string, string[]][] = [
  [
    "https://lacisorg.onmicrosoft.com/EcommerceApi",
    ["https://lacisorg.onmicrosoft.com/EcommerceApi/myscope"]
  ],
  ["https://graph.microsoft.com/v1.0/me", ["user.read"]],
  ["https://graph.microsoft.com/v1.0/user", ["user.read", "user.read.all"]]
];
// :-v[49j@N0IKc2rX]fLE5pq1v?N3[Ah7

export function loggerCallback(logLevel, message, piiEnabled) {
  console.log(logLevel, message, piiEnabled);
}

@NgModule({
  declarations: [
    AppComponent,
    MyProfileComponent,
    ProductComponent,
    ProductDetailComponent,
    PkceComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    MsalModule.forRoot({
      auth: {
        clientId: "383fa7df-2219-4cc4-aedd-95df5ebef58e",
        authority:
          "https://login.microsoftonline.com/a6c5d2d9-0864-4a8e-a168-edb2f8c5ca81/",
        validateAuthority: true,
        redirectUri: "http://localhost:4200/",
        postLogoutRedirectUri: "http://localhost:4200/"
      },
      cache: {
        cacheLocation: "localStorage",
        storeAuthStateInCookie: true
      },
      framework: {
        unprotectedResources: ["https://www.microsoft.com/en-us/"],
        protectedResourceMap: new Map(protectedResourceMap)
      },
      system: {
        logger: new Logger(loggerCallback, {
          level: LogLevel.Verbose,
          piiLoggingEnabled: true
        })
      }
    })
  ],
  providers: [
    ProductService,
    { provide: HTTP_INTERCEPTORS, useClass: MsalInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
