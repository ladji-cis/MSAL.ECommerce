export interface Product {
  id: number;
  name: string;
  price: number;
  categoryName: Category;
}

export interface Category {
  id: number;
  name: string;
}
