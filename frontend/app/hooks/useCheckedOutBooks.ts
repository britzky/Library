import { useState, useEffect } from 'react';
import { Book } from './useFeaturedBooks';

export interface CheckedOutBook extends Book {
  checkoutDate: string;
  dueDate: string;
  transactionId: number;
}

export const useCheckedOutBooks = () => {
  const [checkedOutBooks, setCheckedOutBooks] = useState<CheckedOutBook[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const fetchCheckedOutBooks = async () => {
    setIsLoading(true);
    setError(null);
    try {
      const response = await fetch('http://localhost:5156/api/transaction/checked-out-books', {
        headers: {
          'Authorization': `Bearer ${localStorage.getItem('token')}`
        }
      });
      if (!response.ok) {
        throw new Error('Failed to fetch checked out books');
      }
      const data: CheckedOutBook[] = await response.json();
      setCheckedOutBooks(data);
    } catch (error) {
      setError('Error fetching checked out books');
      console.error('Error fetching checked out books:', error);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchCheckedOutBooks();
  }, []);

  const returnBook = async (transactionId: number) => {
    try {
      const response = await fetch(`http://localhost:5156/api/transaction/customer-return/${transactionId}`, {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${localStorage.getItem('token')}`
        }
      });
      if (!response.ok) {
        throw new Error('Failed to return book');
      }
      // Refetch the checked out books to update the list
      fetchCheckedOutBooks();
    } catch (error) {
      console.error('Error returning book:', error);
      throw error;
    }
  };

  return { checkedOutBooks, isLoading, error, fetchCheckedOutBooks, returnBook };
};