import { PaginationParams } from "./paginationParams";
import { User } from "./user";

export class UserParams extends PaginationParams {
    gender: string;
    minAge: number;
    maxAge: number;    
    orderBy: string;
    
    constructor(user: User | null) {
        super();
        
        this.gender = user?.gender === 'female' ? 'male': 'female';
        this.minAge = 18;
        this.maxAge = 99;        
        this.orderBy = 'lastActive';
    }
}