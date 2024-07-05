import { useState } from 'react';

export const useCheckoutBook = () => {
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    const checkoutBook = async (bookId: number): Promise<void> => {
        setIsLoading(true);
        try {
            const response = await fetch(`http://localhost:5156/api/transaction/checkout/${bookId}`, {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${localStorage.getItem('token')}`,
                    'Content-Type': 'application/json'
                }
            });
            if (!response.ok) {
                const error: Error = await response.json();
                throw new Error(error.message);
            }
        } catch (err: any) {
            setError(err.message);
        } finally {
            setIsLoading(false);
        }
    }
  return { checkoutBook, isLoading, error};
}
