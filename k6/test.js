import http from "k6/http";
import { sleep, check } from "k6";

export let options = {
  vus: 500,
  duration: "30s",
};

export default function () {
  let res = http.get("http://localhost:5145/weatherforecast");
  check(res, { "status is 200": (r) => r.status === 200 });
  sleep(1);
}
