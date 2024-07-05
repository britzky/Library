'use client';

import React, {useState, useEffect } from 'react';
import { useAuth } from '../contexts/AuthContext';
import BookList from '../components/BookList';
import SearchBar from '../components/SearchBar';
import FilterOptions from '../components/FilterOptions';
import { withAuth } from '../utils/withAuth';
import { useFeaturedBooks, Book, Filter } from '../hooks/useFeaturedBooks';


const Dashboard: React.FC = () => {
    const { user } = useAuth();
    const [filteredBooks, setFilteredBooks] = useState<Book[]>([]);
    const [searchTerm, setSearchTerm] = useState<string>('');

    const { books, isLoading, error, setFilter, refetch } = useFeaturedBooks({
        sortBy: '',
        filterBy: ''
    });

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

    const handleFilterChange = (newFilter: Filter): void => {
        setFilter(newFilter);
    };

    return (
        <div className="container mx-auto px-4 py-8">
            <h1 className="text-3xl font-bold mb-8">Welcome, {user?.userName}</h1>
            <div className="mb-8">
                <SearchBar onSearch={handleSearch} />
            </div>
            <div className="flex flex-col md:flex-row gap-8">
                <div className="w-full md:w-1/4">
                    <FilterOptions filter={{sortBy: '', filterBy: '' }} onFilterChange={handleFilterChange} />
                </div>
                <div className="w-full md:w-3/4">
                    {isLoading ? (
                        <p>Loading books...</p>
                    ) : error ? (
                        <p className="text-red-500">{error}</p>
                    ) : (
                        <BookList books={searchTerm ? filteredBooks : books} onBookUpdate={refetch} />
                    )}
                </div>
            </div>
        </div>
    )
}

export default withAuth(Dashboard);