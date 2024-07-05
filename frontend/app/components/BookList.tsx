'use client';

import React from 'react';
import Image from 'next/image';

interface Book {
    id: number;
    title: string;
    author: string;
    description: string;
    coverImage: string;
    availability: boolean;
}

interface BookListProps {
    books: Book[];
}

const BookList: React.FC<BookListProps> = ({ books }) => {
    return (
        <div className = 'grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6'>
            {books.map((book) => (
                <div key={book.id} className="bg-white rounded-lg shadow-md overflow-hidden">
                    <div className="relative h-48">
                        <Image
                            src={book.coverImage}
                            alt={book.title}
                            layout="fill"
                            objectFit="cover"
                        />
                    </div>
                    <div className="p-4">
                        <h3 className="text-lg font-semibold mb-2">{book.title}</h3>
                        <p className="text-sm text-gray-600 mb-2">by {book.author}</p>
                        <p className="text-sm text-gray-800 mb-4 line-clamp-3">{book.description}</p>
                        <div className="flex justify-between items-center">
                            <span className={`px-2 py-1 rounded-full text-xs font-semi-bold ${book.availability ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'}`}>
                              {book.availability ? 'Available' : 'Unavailable'}
                            </span>
                            <button className="px-3 py-1 bg-blue-500 text-white rounded-md hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-500">
                                Details
                            </button>
                        </div>
                    </div>
                </div>
            ))}
        </div>
    )
}

export default BookList;