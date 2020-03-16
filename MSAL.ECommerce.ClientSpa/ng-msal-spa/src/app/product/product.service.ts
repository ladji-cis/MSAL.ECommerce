import { Injectable } from "@angular/core";
import { Subscription, Observable } from "rxjs";
import { map } from "rxjs/operators";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Product } from "./product";
import { MsalService } from "@azure/msal-angular";

@Injectable({
  providedIn: "root"
})
export class ProductService {
  private subscriptions: Subscription[] = [];

  private apiUrl = "https://localhost:44316";

  constructor(private http: HttpClient, private authService: MsalService) {}

  getAllProducts(): Observable<Product[]> {
    let headers = new HttpHeaders().set(
      "Authorization",
      "Bearer " + localStorage.getItem("access_token")
    );
    return this.http
      .get(this.apiUrl + "/api/products", { headers })
      .pipe(map((data: Product[]) => data));
  }
}
