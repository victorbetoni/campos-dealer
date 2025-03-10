import { APIRequest, POST, type Handler } from "./api";

export function dataLoad(handler: Handler<any>) {
  POST(new APIRequest("dataLoad", null, null, null), handler)
}
