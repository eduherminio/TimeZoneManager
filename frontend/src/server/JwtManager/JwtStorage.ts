export class JwtStorage {
    key: string = 'JWT_TOKEN';
    storage: Storage = window.localStorage;

    public constructor(storage: Storage | null) {
        this.storage = storage ? storage : window.localStorage;
    }

    public saveToken(token: string): void {
        this.set(this.key, token);
    }

    public readToken(): string {
        return this.get(this.key);
    }

    public removeToken(): void {
        this.remove(this.key);
    }

    private get(key: string): string {
        let k : string | null = this.storage.getItem(key);        
        return k ? k : ''
    }

    private set(key: string, value: string): void {
        this.storage.setItem(key, value);
    }

    private remove(key: string): void {
        this.storage.removeItem(key);
    }
}
