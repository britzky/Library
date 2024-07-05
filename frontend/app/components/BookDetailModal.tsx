import React, { useState } from 'react';
import { Description, Dialog, DialogPanel, DialogTitle } from '@headlessui/react';
import Image from 'next/image';

interface BookDetail {
    isbn: number;
    title: string;
    author: string;
    description: string;
    coverImage: string;
    publisher: string;
    publicationDate: string;
    category: string;
    pageCount: number;
    availability: boolean;
}

interface BookDetailModalProps {
    bookDetail: BookDetail | null;
    isOpen: boolean;
    onClose: () => void;
}

const BookDetailModal: React.FC<BookDetailModalProps> = ({ bookDetail, isOpen, onClose }) => {
    if (!bookDetail) return null;

  return (
    <>
        <button onClick={() => onClose()} className="text-sm text-blue-700 hover:text-blue-900">
            Close
        </button>
        <Dialog open={isOpen} onClose={onClose} className="relative z-50">
            <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4">
                <DialogPanel className="max-w-lg space-y-4 rounded bg-white p-6 shadow-lg">
                    <Image
                        src={bookDetail.coverImage}
                        alt={`Cover of ${bookDetail.title}`}
                        width={500}
                        height={300}
                        className="rounded"
                    />
                    <DialogTitle className="text-lg font-bold">{bookDetail.title}</DialogTitle>
                    <Description as="div" className="space-y-2">
                        <p><strong>Author:</strong> {bookDetail.author}</p>
                        <p><strong>Description:</strong> {bookDetail.description}</p>
                        <p><strong>Publisher:</strong> {bookDetail.publisher}</p>
                        <p><strong>Publication Date:</strong> {new Date(bookDetail.publicationDate).toLocaleDateString()}</p>
                        <p><strong>ISBN:</strong> {bookDetail.isbn}</p>
                        <p><strong>Category:</strong> {bookDetail.category}</p>
                        <p><strong>Page Count:</strong> {bookDetail.pageCount}</p>
                        <p><strong>Availability:</strong> {bookDetail.availability ? 'Available' : 'Unavailable'}</p>
                    </Description>
                    <div className="flex justify-end gap-4">
                        <button onClick={onClose} className="px-4 py-2 bg-gray-300 hover:bg-gray-400 rounded text-black">
                            Close
                        </button>
                    </div>
                </DialogPanel>
            </div>
        </Dialog>
    </>
  )
}

export default BookDetailModal;
