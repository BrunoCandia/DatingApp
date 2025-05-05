import { PaginationParams } from "./paginationParams";

export class MessageParams extends PaginationParams {
    container: string;
    
    constructor() {
        super();
        
        this.container = 'Unread';                
    }
}