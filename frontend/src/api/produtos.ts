import type Produto from "../model/Produto";
import { APIRequest, DELETE, GET, POST, PUT, type Handler } from "./api";

const PRODUTO_CONTROLLER = "produto"

export function createProduto(desc: string, price: number, handler: Handler<Produto>) {
  POST(new APIRequest(PRODUTO_CONTROLLER, null, ({
    description: desc,
    unitaryprice: price
  }), null), handler)
}

export function fetchProdutos(page: number, description: string, handler: Handler<Produto[]>) {
  GET(new APIRequest(PRODUTO_CONTROLLER, null, null, ({
    desc: description,
    page: page
  })), handler)
}

export function updateProduto(c: Produto, handler: Handler<Produto>) {
  PUT(new APIRequest(PRODUTO_CONTROLLER, null, c, null), handler)
};

export function deleteProduto(id: number, handler: Handler<any>) {
  DELETE(new APIRequest(PRODUTO_CONTROLLER, null, ({id: id}), null), handler)
}