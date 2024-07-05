import { useState, useEffect } from 'react';

export interface Book {
    id: number;
    title: string;
    author: string;
    description: string;
    coverImage: string;
    availability: boolean;
}

export interface Filter {
    sortBy: string;
    filterBy: string;
}

export const useFeaturedBooks = (initialFilter: Filter) => {
    const [books, setBooks] = useState<Book[]>([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [filter, setFilter] = useState(initialFilter);

    const fetchBooks = async () => {
        setIsLoading(true);
        setError(null);
        try {
            const queryParams = new URLSearchParams({
                sortBy: filter.sortBy,
                filterBy: filter.filterBy
            }).toString();
            const response = await fetch(`http://localhost:5156/api/book/featured?${queryParams}`, {
                headers: {
                    'Authorization': `Bearer ${localStorage.getItem('token')}`
                }
            });
            if (!response.ok) {
                throw new Error('Failed to fetch books');
            }
            const data = await response.json();
            setBooks(data);
        } catch (error) {
            setError('Error fetching books');
            console.error('Error fetching books:', error);
        } finally {
            setIsLoading(false);
        }
    };

    useEffect(() => {
        fetchBooks();
    }, [filter]);

    return { books, isLoading, error, setFilter, refetch: fetchBooks };
};