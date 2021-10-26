import axios from "axios";

import {
message
} from 'antd';


axios.defaults.withCredentials = true;

const error = () => {
  message.error('Błąd uwierzytelnienia');
};


export async function APIclient({ endpoint, customConfig, method, body } = {}) {

  let headers;
  try {
    headers = {
      "Content-Type": "application/json",
      credentials: 'include',
    }
  } catch (err) {
  }

  const config = {
    method: method,
    url: endpoint,
    ...customConfig,
    headers: {
      ...headers,
      ...customConfig.headers,
    },
    withCredentials: true,
  };

  if (body) {
    config.data = body;
  }

  let data;
  try {

    const response = await axios(config);
    data = response;

    return data;

    // if (response.status < 400) {
    //   return data;
    // }

    // throw new Error(response.statusText);
  } catch (err) {

    return err;
    //  error();
    // return Promise.reject(err.message ? err.message : data);
  }
}

APIclient.get = function ({endpoint}) {
  return APIclient({ endpoint, method: 'GET', customConfig: {} })
}

APIclient.post = function ({ endpoint, body, customConfig }) {
  return APIclient({ endpoint, method: 'POST', body, customConfig })
}

APIclient.put = function ({endpoint, body, customConfig = {}}) {
  return APIclient({ endpoint, method: 'PUT', body, customConfig })
}

APIclient.delete = function ({endpoint, body, customConfig = {}}) {
  return APIclient({ endpoint, method: 'DELETE', body, customConfig })
}

