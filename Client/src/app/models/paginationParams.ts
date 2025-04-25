export class PaginationParams {
    pageNumber: number;
    pageSize: number;
    
    constructor(pageNumber: number = 1, pageSize: number = 5) {
        this.pageNumber = pageNumber;
        this.pageSize = pageSize;
    }
}