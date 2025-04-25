import { PaginationParams } from "./paginationParams";

export class LikeParams extends PaginationParams {    
    predicate: string;
    
    constructor() {
        super();
        
        this.predicate = 'liked';                
    }
}