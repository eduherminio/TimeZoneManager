import { JwtStorage } from './JwtManager/JwtStorage';
const jwtStorage = new JwtStorage(null);

export class BaseClient {

  transformOptions(options: RequestInit): Promise<RequestInit> {
      let newHeaders: Headers = new Headers(options.headers);
      newHeaders.append('Authorization', `Bearer ${jwtStorage.readToken()}`);
      options.headers = newHeaders;
      return Promise.resolve(options);
  }

  getBaseUrl(noidea: string, defaultUrl: string | undefined): string {
    return 'http://localhost:8001';
}

  transformResult(url: string, response: Response, processor: (response: Response) => Promise<any>): Promise<any> { 
    return processor(response);
  }
}

