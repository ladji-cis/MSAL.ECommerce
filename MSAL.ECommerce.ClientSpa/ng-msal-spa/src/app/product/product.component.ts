import { Component, OnInit, OnDestroy } from "@angular/core";
import { ProductService } from "./product.service";
import { Product } from "./product";
import { Subscription } from "rxjs";

@Component({
  selector: "app-product",
  templateUrl: "./product.component.html",
  styleUrls: ["./product.component.css"]
})
export class ProductComponent implements OnInit, OnDestroy {
  private products: Product[] = [];
  private subscriptions: Subscription[] = [];

  constructor(private productService: ProductService) {}

  ngOnInit() {
    this.subscriptions.push(
      this.productService.getAllProducts().subscribe(
        data => (this.products = data),
        err => console.error(err)
      )
    );
  }

  ngOnDestroy(): void {
    if (this.subscriptions) {
      this.subscriptions.forEach(sub => sub.unsubscribe());
    }
  }
}
