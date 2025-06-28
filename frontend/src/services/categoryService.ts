import { apiService } from "./apiService";
import type { ApiResponse } from "@/types/api";
import type { Category, CategoryFormData } from "@/types/category";

export class CategoryService {
    async getAll(): Promise<ApiResponse<Category[]>> {
        return apiService.get<Category[]>("/Category");
    }
    async create(data: CategoryFormData): Promise<ApiResponse<Category>> {
        return apiService.post<Category, CategoryFormData>("/Category", data);
    }
    async update(id: number, data: CategoryFormData): Promise<ApiResponse<Category>> {
        return apiService.put<Category, CategoryFormData>(`/Category/${id}`, data);
    }
}

export const categoryService = new CategoryService();