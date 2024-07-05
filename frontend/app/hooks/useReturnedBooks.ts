// hooks/useReturnedBooks.ts

import { useState, useEffect } from 'react';

interface ReturnedBook {
  id: number;
  title: string;
  author: string;
  checkoutDate: string;
  returnDate: string;
  transactionId: number;
}

export const useReturnedBooks = () => {
  const [returnedBooks, setReturnedBooks] = useState<ReturnedBook[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const fetchReturnedBooks = async () => {
    setIsLoading(true);
    setError(null);
    try {
      const response = await fetch('http://localhost:5156/api/transaction/books-to-return', {
        headers: {
          'Authorization': `Bearer ${localStorage.getItem('token')}`
        }
      });
      if (!response.ok) {
        throw new Error('Failed to fetch returned books');
      }
      const data: ReturnedBook[] = await response.json();
      setReturnedBooks(data);
    } catch (error) {
      setError('Error fetching returned books');
      console.error('Error fetching returned books:', error);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchReturnedBooks();
  }, []);

  const returnToCirculation = async (transactionId: number) => {
    try {
      const response = await fetch(`http://localhost:5156/api/transaction/librarian-return/${transactionId}`, {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${localStorage.getItem('token')}`
        }
      });
      if (!response.ok) {
        throw new Error('Failed to return book to circulation');
      }
      // Refetch the returned books to update the list
      fetchReturnedBooks();
    } catch (error) {
      console.error('Error returning book to circulation:', error);
      throw error;
    }
  };

  return { returnedBooks, isLoading, error, fetchReturnedBooks, returnToCirculation };
};