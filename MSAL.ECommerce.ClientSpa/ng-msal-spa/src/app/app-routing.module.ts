import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { ProductComponent } from "./product/product.component";
import { MsalGuard } from "@azure/msal-angular";
import { ProductDetailComponent } from "./product/product-detail.component";
import { MyProfileComponent } from "./my-profile/my-profile.component";

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
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
