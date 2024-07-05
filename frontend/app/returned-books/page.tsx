'use client';

import React from 'react';
import { withAuth } from '../utils/withAuth';
import { useReturnedBooks } from '../hooks/useReturnedBooks';

interface ReturnedBook {
  id: number;
  title: string;
  author: string;
  checkoutDate: string;
  returnDate: string;
  transactionId: number;
}

const ReturnedBooks: React.FC = () => {
  const { returnedBooks, isLoading, error, returnToCirculation } = useReturnedBooks();

  const handleReturnToCirculation = async (transactionId: number) => {
    try {
      await returnToCirculation(transactionId);
      // Optionally, you can add a success message here
    } catch (error) {
      console.error('Error returning book to circulation:', error);
      // Optionally, you can add an error message here
    }
  };

  return (
    <div className="container mx-auto px-4 py-8">
      <h1 className="text-3xl font-bold mb-8">Books to Return to Circulation</h1>
      {isLoading ? (
        <p>Loading returned books...</p>
      ) : error ? (
        <p className="text-red-500">{error}</p>
      ) : returnedBooks.length === 0 ? (
        <p>There are no books to return to circulation at the moment.</p>
      ) : (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
          {returnedBooks.map((book) => (
            <div key={book.id} className="bg-white rounded-lg shadow-md overflow-hidden">
              <div className="p-4">
                <h3 className="text-lg font-semibold mb-2">{book.title}</h3>
                <p className="text-sm text-gray-600 mb-2">by {book.author}</p>
                <p className="text-sm text-gray-800 mb-2">Checkout Date: {new Date(book.checkoutDate).toLocaleDateString()}</p>
                <p className="text-sm text-gray-800 mb-4">Return Date: {new Date(book.returnDate).toLocaleDateString()}</p>
                <button
                  onClick={() => handleReturnToCirculation(book.transactionId)}
                  className="px-3 py-1 bg-blue-500 text-white rounded-md hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-500"
                >
                  Return to Circulation
                </button>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default withAuth(ReturnedBooks);