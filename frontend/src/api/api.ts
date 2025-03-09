
export interface APIResponse<T> {
  status: number;
  message: string | null;
  errors: string[];
  data: T | null;
}

function responseFrom<T>(j:any): APIResponse<T> {
  return j as APIResponse<T>
}

export class APIRequest<T> {
  public route: string;
  public endpoint: string | null = null; 
  public body: T | null;
  public query: T | null;

  constructor(route: string, endpoint: string | null, body: T | null, query: T | null) {
    this.route = route;
    this.endpoint = endpoint ?? import.meta.env.VITE_API_HOST
    this.body = body;
    this.query = query;  
  }
}

const headers = {
  "Content-Type": "application/json"
}

export type Handler<T> = (resp: APIResponse<T>) => void;

export function POST<A,T>(req: APIRequest<A>, handler: Handler<T>) {
  fetch(`${req.endpoint}/${req.route}`,{
    body: req.body == null ? "{}" : JSON.stringify(req.body),
    method: "POST",
    headers: headers,
  }).then(x => x.json()).then(j => handler(responseFrom(j)))
}

export function PUT<A,T>(req: APIRequest<A>, handler: Handler<T>) {
  fetch(`${req.endpoint}/${req.route}`,{
    body: req.body == null ? "{}" : JSON.stringify(req.body),
    method: "PUT",
    headers: headers,
  }).then(x => x.json()).then(j => handler(responseFrom(j)))
}

export function GET<A,T>(req: APIRequest<A>, handler: Handler<T>) {
  let filtered: Record<string,any> = req.query == null ? {} : Object.fromEntries(
    Object.entries(req.query).filter(([_, value]) => value != null && value != undefined && value != "")
  );
  fetch(`${req.endpoint}/${req.route}${req.query != null ? "?" + new URLSearchParams(filtered).toString() : ""}`, {
    method: "GET",
    headers: headers,
  }).then(x => x.json()).then(j => handler(responseFrom(j)))
}

export function DELETE<A,T>(req: APIRequest<A>, handler: Handler<T>) {
  let filtered: Record<string,any> = req.query == null ? {} : Object.fromEntries(
    Object.entries(req.query).filter(([_, value]) => value != null && value != undefined && value != "")
  );
  fetch(`${req.endpoint}/${req.route}${req.query != null ? "?" + new URLSearchParams(filtered).toString() : ""}`, {
    method: "DELETE",
    body: req.body == null ? "{}" : JSON.stringify(req.body),
    headers: headers,
  }).then(x => x.json()).then(j => handler(responseFrom(j)))
}