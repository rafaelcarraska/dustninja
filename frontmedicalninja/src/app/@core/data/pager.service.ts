
export class PagerService {
    getPager(totalItems: number, currentPage: number = 1, pageSize: number = 10) {
        // calculando o total de paginas
        let totalPages = Math.ceil(totalItems / pageSize);

        let startPage: number, endPage: number;
        if (totalPages <= 10) {
            // Se tiver menos de 10 páginas totais para mostrar tudo
            startPage = 1;
            endPage = totalPages;
        } else {
            // Se tiver mais de 10 páginas totais, calcular as páginas inicial e final
            if (currentPage <= 6) {
                startPage = 1;
                endPage = 10;
            } else if (currentPage + 4 >= totalPages) {
                startPage = totalPages - 9;
                endPage = totalPages;
            } else {
                startPage = currentPage - 5;
                endPage = currentPage + 4;
            }
        }

        // Cálculo de índices de itens iniciais e finais
        let startIndex = (currentPage - 1) * pageSize;
        let endIndex = Math.min(startIndex + pageSize - 1, totalItems - 1);

        // Criar um array de páginas para o ng-repeat no componente
        // let pages = _.range(startPage, endPage + 1);
        let pages = [];
        for (let index = startPage; index < endPage + 1; index++) {
          pages.push(index);
        }


        // Retornar objeto com todas as propriedades exigidas para exibição
        return {
            totalItems: totalItems,
            currentPage: currentPage,
            pageSize: pageSize,
            totalPages: totalPages,
            startPage: startPage,
            endPage: endPage,
            startIndex: startIndex,
            endIndex: endIndex,
            pages: pages
        };
    }
}
