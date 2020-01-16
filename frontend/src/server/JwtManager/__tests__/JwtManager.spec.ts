import { JwtManager, JwtTokenPayload } from "../JwtManager";

const jwtManager = new JwtManager(null);

describe("JwtManager unit testing", () => {
  beforeEach(() => {
    jwtManager.forget();
  });

  test("Save valid token", () => {
    // tslint:disable-next-line:max-line-length
    let rawToken: string =
      "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiYWRtaW4iLCJwcm1zIjpbIkFkbWluIl0sIm5iZiI6MTU3OTAyNzg4OCwiZXhwIjoxNTc5MDM1MDg4LCJpYXQiOjE1NzkwMjc4ODh9.KugS62lxOBmKXlYbzZSbMX7lIBp6rNe6EB1hUh4tOzw";
    let payload: JwtTokenPayload = jwtManager.save(rawToken);

    let rawReadToken: string = jwtManager.getRawToken();
    expect(rawReadToken).toBe(rawToken);

    expect(payload.username).toBe("admin");
    expect(payload.permissions).toHaveLength(1);
    expect(payload.permissions[0]).toBe("Admin");
  });

  test("Validate token", () => {
    expect(jwtManager.validateToken()).toBe(false);

    // tslint:disable-next-line:max-line-length
    let rawToken: string =
      "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiYWRtaW4iLCJwcm1zIjpbIkFkbWluIl0sIm5iZiI6MTU3OTAyNzg4OCwiZXhwIjoxNTc5MDM1MDg4LCJpYXQiOjE1NzkwMjc4ODh9.KugS62lxOBmKXlYbzZSbMX7lIBp6rNe6EB1hUh4tOzw";
    jwtManager.save(rawToken);
    expect(jwtManager.validateToken()).toBe(true);
  });

  test("Should not save invalid token", () => {
    // tslint:disable-next-line:max-line-length
    let rawToken: string =
      "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiYWRtaW4iLCJwZXJtaXNzaW9ucyI6WyJBZG1pbiJdLCJuYmYiOjU3OTAyNzg4OCwiZXhwIjoxNTc5MDM1MDg4LCJpYXQzzzziOjE1NzkwMjc4ODh9.e8bWQazf4P4LXxZMIWB-Fgh9D-UdUkIc7bGoN8UC-icA";
    expect(() => jwtManager.save(rawToken)).toThrowError(Error);
    expect(jwtManager.getRawToken()).toBe("");
  });
});
