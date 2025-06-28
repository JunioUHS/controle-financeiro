export interface ApiError {
    field: string;
    message: string;
}

export interface ApiResponse<T = never> {
    success: boolean;
    data?: T;
    message?: string;
    errors?: ApiError[];
}

export interface PaginationMetadata {
    totalItems: number;
    page: number;
    pageSize: number;
    totalPages: number;
}

export interface ApiPagedResponse<T> extends ApiResponse<T[]> {
    meta: PaginationMetadata;
}