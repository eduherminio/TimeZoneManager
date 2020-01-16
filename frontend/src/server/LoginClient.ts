import { LoginClient  as LoginGeneratedClient, SwaggerException } from './autogeneratedclients/TimeZoneManagerClient';
import { JwtManager } from './JwtManager/JwtManager';
export type SwaggerException = SwaggerException;

const jwtManager = new JwtManager(null);

export class LoginClient {

    private readonly client: LoginGeneratedClient;

    public constructor() {
        this.client = new LoginGeneratedClient();
    }

    public login(login: string, password: string): Promise<string> {
        return this.client.index(login, password);
    }

    public renewToken(): Promise<string> {
        if (jwtManager.getRawToken()) {
            return this.client.renewToken();
        }
        return Promise.resolve('');
    }
}