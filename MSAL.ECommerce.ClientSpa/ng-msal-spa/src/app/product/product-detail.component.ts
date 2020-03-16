import { Component, OnInit, Input } from "@angular/core";
import { ActivatedRoute } from "@angular/router";

@Component({
  selector: "app-product-detail",
  template: `
    <div>
      <h2>Product Detail: {{ product.id }}</h2>
    </div>
  `,
  styles: [""]
})
export class ProductDetailComponent implements OnInit {
  @Input() product: any;

  constructor(private aRoute: ActivatedRoute) {
    //let id: string = aRoute.params["id"];
  }

  ngOnInit(): void {}
}
