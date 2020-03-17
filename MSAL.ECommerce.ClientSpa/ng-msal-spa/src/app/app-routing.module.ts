import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { ProductComponent } from "./product/product.component";
import { MsalGuard } from "@azure/msal-angular";
import { ProductDetailComponent } from "./product/product-detail.component";
import { MyProfileComponent } from "./my-profile/my-profile.component";
import { PkceComponent } from "./pkce/pkce.component";

const routes: Routes = [
  {
    path: "product",
    component: ProductComponent,
    canActivate: [MsalGuard],
    children: [{ path: "detail/:id", component: ProductDetailComponent }]
  },
  {
    path: "myprofile",
    component: MyProfileComponent,
    canActivate: [MsalGuard]
  },
  {
    path: "pkce",
    component: PkceComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
