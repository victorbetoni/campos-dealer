import type Produto from "../model/Produto";
import { APIRequest, DELETE, GET, POST, PUT, type Handler } from "./api";

const PRODUTO_CONTROLLER = "produto"

export function createProduct(desc: string, price: number, handler: Handler<Produto>) {
  POST(new APIRequest(PRODUTO_CONTROLLER, null, ({
    description: desc,
    unitaryprice: price
  }), null), handler)
}

export function fetchProducts(page: number, description: string, handler: Handler<Produto[]>) {
  GET(new APIRequest(PRODUTO_CONTROLLER, null, null, ({
    desc: description,
    page: page
  })), handler)
}

export function fetchAllProducts(desc: string, handler: Handler<Produto[]>) {
  fetchProducts(-1, desc, handler);
}


export function updateProduct(c: Produto, handler: Handler<Produto>) {
  PUT(new APIRequest(PRODUTO_CONTROLLER, null, c, null), handler)
};

export function deleteProduct(id: number, handler: Handler<any>) {
  DELETE(new APIRequest(PRODUTO_CONTROLLER, null, ({id: id}), null), handler)
}