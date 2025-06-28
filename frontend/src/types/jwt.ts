export interface JWTClaims {
    sub?: string;
    nameid?: string;
    unique_name?: string;
    username?: string;
    given_name?: string;
    name?: string;
    email?: string;
    exp?: number;
    iat?: number;
    iss?: string;
    aud?: string;
}