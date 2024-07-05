'use client';

import { withAuth } from '../utils/withAuth';
import { useCheckedOutBooks } from '../hooks/useCheckedOutBooks';

const ReturnBooks: React.FC = () => {
    const { checkedOutBooks, isLoading, error, returnBook } = useCheckedOutBooks();

    return (
        <div className="container mx-auto px-4 py-8">
            <h1 className="text-3xl font-bold mb-8">Your Checked Out Books</h1>
            {isLoading ? (
            <p>Loading checked out books...</p>
            ) : error ? (
            <p className="text-red-500">{error}</p>
            ) : (
            <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
                {checkedOutBooks.map((book) => (
                <div key={book.id} className="bg-white rounded-lg shadow-md overflow-hidden">
                    <div className="p-4">
                    <h3 className="text-lg font-semibold mb-2">{book.title}</h3>
                    <p className="text-sm text-gray-600 mb-2">by {book.author}</p>
                    <p className="text-sm text-gray-800 mb-2">Checkout Date: {new Date(book.checkoutDate).toLocaleDateString()}</p>
                    <p className="text-sm text-gray-800 mb-4">Due Date: {new Date(book.dueDate).toLocaleDateString()}</p>
                    <button
                        onClick={() => returnBook(book.transactionId)}
                        className="px-3 py-1 bg-blue-500 text-white rounded-md hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-500"
                    >
                        Return Book
                    </button>
                    </div>
                </div>
                ))}
            </div>
            )}
        </div>
    )
}

export default withAuth(ReturnBooks);

