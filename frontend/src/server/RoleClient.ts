import {
  RoleClient as RoleGeneratedClient,
  RoleDto
} from "./autogeneratedclients/TimeZoneManagerClient";
export type RoleDto = RoleDto;

export class RoleClient {
  private readonly client: RoleGeneratedClient;

  public constructor() {
    this.client = new RoleGeneratedClient();
  }

  public loadAll(): Promise<Array<RoleDto>> {
    return this.client.loadAll();
  }
}
