'use client';

import { useState } from 'react';
import Link from 'next/link';
import ExamSummaryCard from '@/components/ExamSummaryCard';
import { getStudentAnalytics } from '@/lib/api';
import { ExamSummary } from '@/lib/types';

export default function StudentPage() {
  const [studentIdInput, setStudentIdInput] = useState('');
  const [studentId, setStudentId] = useState<number | null>(null);
  const [summaries, setSummaries] = useState<ExamSummary[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [hasSearched, setHasSearched] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setLoading(true);
    setHasSearched(true);

    const id = parseInt(studentIdInput);
    if (isNaN(id)) {
      setError('Please enter a valid student ID');
      setLoading(false);
      return;
    }

    setStudentId(id);

    try {
      const data = await getStudentAnalytics(id);
      setSummaries(data);
    } catch (err: any) {
      setError(err.message || 'Failed to load analytics');
      setSummaries([]);
    } finally {
      setLoading(false);
    }
  };

  const totalExams = summaries.length;
  const averageScore = totalExams > 0
    ? summaries.reduce((sum, s) => sum + s.scorePercentage, 0) / totalExams
    : 0;
  const totalTasks = summaries.reduce((sum, s) => sum + s.totalTasks, 0);
  const totalCorrect = summaries.reduce((sum, s) => sum + s.correctTasks, 0);

  return (
    <div className="container mx-auto px-4 py-12">
      <div className="max-w-6xl mx-auto">
        <div className="flex justify-between items-center mb-8">
          <h1 className="text-4xl font-bold">Student Analytics</h1>
          <Link
            href="/"
            className="px-4 py-2 bg-slate-600 text-white rounded-lg hover:bg-slate-700 transition-colors"
          >
            Home
          </Link>
        </div>

        <div className="bg-white p-6 rounded-lg shadow-md mb-8">
          <h2 className="text-xl font-semibold mb-4">Enter Your Student ID</h2>
          <form onSubmit={handleSubmit} className="flex gap-4">
            <input
              type="text"
              value={studentIdInput}
              onChange={(e) => setStudentIdInput(e.target.value)}
              placeholder="e.g., 10001"
              className="flex-1 px-4 py-2 border border-slate-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-600"
              required
            />
            <button
              type="submit"
              disabled={loading}
              className="px-6 py-2 bg-blue-600 text-white rounded-lg font-semibold hover:bg-blue-700 disabled:bg-slate-400 transition-colors"
            >
              {loading ? 'Loading...' : 'View Results'}
            </button>
          </form>
        </div>

        {error && (
          <div className="bg-red-50 border-2 border-red-200 rounded-lg p-6 mb-8">
            <p className="text-red-600">{error}</p>
          </div>
        )}

        {hasSearched && !loading && !error && (
          <>
            <p className="text-xl text-slate-600 mb-6">Results for Student ID: {studentId}</p>

            {summaries.length > 0 ? (
              <>
                <div className="grid grid-cols-1 md:grid-cols-4 gap-6 mb-12">
                  <div className="bg-white p-6 rounded-lg shadow-md">
                    <p className="text-sm text-slate-600 mb-2">Total Exams</p>
                    <p className="text-3xl font-bold">{totalExams}</p>
                  </div>
                  <div className="bg-white p-6 rounded-lg shadow-md">
                    <p className="text-sm text-slate-600 mb-2">Average Score</p>
                    <p className={`text-3xl font-bold ${averageScore >= 70 ? 'text-green-600' : averageScore >= 50 ? 'text-yellow-600' : 'text-red-600'}`}>
                      {averageScore.toFixed(1)}%
                    </p>
                  </div>
                  <div className="bg-white p-6 rounded-lg shadow-md">
                    <p className="text-sm text-slate-600 mb-2">Total Tasks</p>
                    <p className="text-3xl font-bold">{totalTasks}</p>
                  </div>
                  <div className="bg-white p-6 rounded-lg shadow-md">
                    <p className="text-sm text-slate-600 mb-2">Correct Answers</p>
                    <p className="text-3xl font-bold text-green-600">{totalCorrect}</p>
                  </div>
                </div>

                <h2 className="text-2xl font-semibold mb-6">Your Exams</h2>
                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                  {summaries.map((summary) => (
                    <ExamSummaryCard key={summary.examId} summary={summary} />
                  ))}
                </div>
              </>
            ) : (
              <div className="bg-slate-50 border-2 border-slate-200 rounded-lg p-8 text-center">
                <h3 className="text-xl font-semibold mb-2">No exams found</h3>
                <p className="text-slate-600">
                  No graded exams found for this student ID. Make sure your teacher has uploaded and graded your exam.
                </p>
              </div>
            )}
          </>
        )}

        {!hasSearched && (
          <div className="bg-blue-50 border-2 border-blue-200 rounded-lg p-8 text-center">
            <h3 className="text-xl font-semibold mb-2">View Your Exam Results</h3>
            <p className="text-slate-600">
              Enter your student ID above to see your graded exams and analytics.
            </p>
          </div>
        )}
      </div>
    </div>
  );
}
