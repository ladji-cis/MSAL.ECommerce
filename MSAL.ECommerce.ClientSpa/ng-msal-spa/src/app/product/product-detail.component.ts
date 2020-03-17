import { Component, OnInit, Input } from "@angular/core";
import { ActivatedRoute, ParamMap } from "@angular/router";
import { Observable } from "rxjs";
import { switchMap } from "rxjs/operators";

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

  private code: Observable<string>;

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {}
}
