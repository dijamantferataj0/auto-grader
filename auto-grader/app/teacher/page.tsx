'use client';

import { useState, useEffect } from 'react';
import { useRouter } from 'next/navigation';
import FileUpload from '@/components/FileUpload';
import ResultsTable from '@/components/ResultsTable';
import { UploadResponse } from '@/lib/types';
import { isAuthenticated, logout } from '@/lib/api';

export default function TeacherPage() {
  const router = useRouter();
  const [uploadResult, setUploadResult] = useState<UploadResponse | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (!isAuthenticated()) {
      router.push('/login?redirect=/teacher');
    } else {
      setLoading(false);
    }
  }, [router]);

  const handleLogout = () => {
    logout();
    router.push('/login');
  };

  const handleUploadSuccess = (response: UploadResponse) => {
    setUploadResult(response);
  };

  if (loading) {
    return (
      <div className="container mx-auto px-4 py-12">
        <div className="text-center">
          <div className="text-4xl mb-4">⏳</div>
          <p className="text-xl">Loading...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="container mx-auto px-4 py-12">
      <div className="max-w-6xl mx-auto">
        <div className="flex justify-between items-center mb-8">
          <h1 className="text-4xl font-bold">Teacher Dashboard</h1>
          <button
            onClick={handleLogout}
            className="px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700 transition-colors"
          >
            Logout
          </button>
        </div>

        <div className="mb-12">
          <h2 className="text-2xl font-semibold mb-6">Upload Student Exams</h2>
          <FileUpload onUploadSuccess={handleUploadSuccess} />
        </div>

        {uploadResult && (
          <div className="space-y-8">
            <div className="bg-white p-6 rounded-lg shadow-md">
              <h3 className="text-xl font-semibold mb-4">Upload Summary</h3>
              <div className="grid grid-cols-3 gap-4">
                <div>
                  <p className="text-sm text-slate-600">Teacher ID</p>
                  <p className="text-2xl font-bold">{uploadResult.teacherId}</p>
                </div>
                <div>
                  <p className="text-sm text-slate-600">Students Processed</p>
                  <p className="text-2xl font-bold">{uploadResult.processedStudents}</p>
                </div>
                <div>
                  <p className="text-sm text-slate-600">Total Exams</p>
                  <p className="text-2xl font-bold">{uploadResult.totalExams}</p>
                </div>
              </div>
            </div>

            <div>
              <h3 className="text-xl font-semibold mb-4">Grading Results</h3>
              <ResultsTable summaries={uploadResult.summaries} />
            </div>
          </div>
        )}

        {!uploadResult && (
          <div className="bg-blue-50 border-2 border-blue-200 rounded-lg p-8 text-center">
            <div className="text-4xl mb-4">📝</div>
            <h3 className="text-xl font-semibold mb-2">No exams uploaded yet</h3>
            <p className="text-slate-600">
              Upload an XML file to see grading results and analytics
            </p>
          </div>
        )}
      </div>
    </div>
  );
}
