'use client';

import React, {useState, useEffect } from 'react';
import { useAuth } from '../contexts/AuthContext';
import BookList from '../components/BookList';
import SearchBar from '../components/SearchBar';
import FilterOptions from '../components/FilterOptions';
import { withAuth } from '../utils/withAuth';

interface Book {
    id: number;
    title: string;
    author: string;
    description: string;
    coverImage: string;
    availability: boolean;
}

const Dashboard: React.FC = () => {
    const { user } = useAuth();
    const [books, setBooks] = useState<Book[]>([]);
    const [filteredBooks, setFilteredBooks] = useState<Book[]>([]);
    const [searchTerm, setSearchTerm] = useState('');
    const [filter, setFilter] = useState({
        sortBy: '',
        filterBy: ''
    });

    useEffect(() => {
        fetchFeaturedBooks();
    }, [filter]);

    const fetchFeaturedBooks = async () => {
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
            setFilteredBooks(data);
        } catch (error) {
            console.error('Error fetching books:', error);
        }
    }

    const handleSearch = async (term: string) => {
        setSearchTerm(term);
        if (term) {
            try {
                const response = await fetch(`http://localhost:5156/api/book/search?searchTerm=${term}`, {
                    headers: {
                        'Authorization': `Bearer ${localStorage.getItem('token')}`
                    }
                })
                if (!response.ok) {
                    throw new Error('Search failed');
                }
                const data = await response.json();
                setFilteredBooks(data);
            } catch (error) {
                console.error('Search error:', error);
            }
        } else {
            setFilteredBooks(books);
        }
    };

    return (
        <div className="container mx-auto px-4 py-8">
            <h1 className="text-3xl font-bold mb-8">Welcome, {user?.userName}</h1>
            <div className="mb-8">
                <SearchBar onSearch={handleSearch} />
            </div>
            <div className="flex flex-col md:flex-row gap-8">
                <div className="w-full md:w-1/4">
                    <FilterOptions filter={filter} onFilterChange={setFilter} />
                </div>
                <div className="w-full md:w-3/4">
                    <BookList books={filteredBooks} />
                </div>
            </div>
        </div>
    )
}

export default withAuth(Dashboard);