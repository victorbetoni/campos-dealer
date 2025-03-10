import type Cliente from "./Cliente";
import type Produto from "./Produto";

export default interface Venda {
  id: number;
  customer: Cliente;
  product: Produto;
  quantity: number;
  unitaryValue: number;
  total: number;
  date: number;

}