import {
  JwtTokenPayload,
  CustomClaimTypesName
} from "../autogeneratedclients/TimeZoneManagerClient";
import { JwtStorage } from "./JwtStorage";
export type JwtTokenPayload = JwtTokenPayload;

export class JwtManager {
  storage: JwtStorage;

  public constructor(storage: JwtStorage | null) {
    this.storage = storage ? storage : new JwtStorage(null);
  }

  public save(rawToken: string): JwtTokenPayload {
    let payload: JwtTokenPayload = this.read(rawToken);
    this.storage.saveToken(rawToken);
    return payload;
  }

  public getRawToken(): string {
    return this.storage.readToken();
  }

  public forget(): void {
    this.storage.removeToken();
  }

  public validateToken(): boolean {
    return this.storage.readToken() !== null && this.storage.readToken() !== "";
  }

  private read(value: string): JwtTokenPayload {
    try {
      let parts: string[] = value.split(".");
      let rawPayload = JSON.parse(this.decode64(parts[1]));

      return {
        username: rawPayload[CustomClaimTypesName.Name],
        permissions: this.getArray(rawPayload[CustomClaimTypesName.Prms])
      };
    } catch (e) {
      throw Error("Invalid token");
    }
  }

  private decode64(value: string): string {
    return decodeURIComponent(escape(atob(value)));
  }

  private getArray<T>(values: T | T[]): T[] {
    if (!values) {
      return [];
    } else if (values instanceof Array) {
      return [...values];
    } else {
      return [values];
    }
  }
}