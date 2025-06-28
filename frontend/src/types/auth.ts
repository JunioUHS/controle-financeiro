export interface LoginUserDto {
    userName: string;
    password: string;
}

export interface RegisterUserDto {
    userName: string;
    fullName: string;
    password: string;
    confirmPassword: string;
}

export interface ApplicationUser {
    id: string;
    userName: string;
    fullName: string;
    email?: string;
}

// Tipos para componentes
export type LoginCredentials = LoginUserDto;
export type RegisterCredentials = RegisterUserDto;
export type User = ApplicationUser;