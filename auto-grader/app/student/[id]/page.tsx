'use client';

import { useEffect, useState } from 'react';
import { useParams } from 'next/navigation';
import ExamSummaryCard from '@/components/ExamSummaryCard';
import { getStudentAnalytics } from '@/lib/api';
import { ExamSummary } from '@/lib/types';

export default function StudentPage() {
  const params = useParams();
  const studentId = parseInt(params.id as string);

  const [summaries, setSummaries] = useState<ExamSummary[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchAnalytics = async () => {
      try {
        setLoading(true);
        const data = await getStudentAnalytics(studentId);
        setSummaries(data);
      } catch (err: any) {
        setError(err.message || 'Failed to load analytics');
      } finally {
        setLoading(false);
      }
    };

    fetchAnalytics();
  }, [studentId]);

  if (loading) {
    return (
      <div className="container mx-auto px-4 py-12">
        <div className="text-center">
          <div className="text-4xl mb-4">⏳</div>
          <p className="text-xl">Loading analytics...</p>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="container mx-auto px-4 py-12">
        <div className="bg-red-50 border-2 border-red-200 rounded-lg p-8 text-center">
          <div className="text-4xl mb-4">❌</div>
          <h2 className="text-2xl font-bold mb-2">Error</h2>
          <p className="text-red-600">{error}</p>
        </div>
      </div>
    );
  }

  const totalExams = summaries.length;
  const averageScore = totalExams > 0
    ? summaries.reduce((sum, s) => sum + s.scorePercentage, 0) / totalExams
    : 0;
  const totalTasks = summaries.reduce((sum, s) => sum + s.totalTasks, 0);
  const totalCorrect = summaries.reduce((sum, s) => sum + s.correctTasks, 0);

  return (
    <div className="container mx-auto px-4 py-12">
      <div className="max-w-6xl mx-auto">
        <h1 className="text-4xl font-bold mb-2">Student Analytics</h1>
        <p className="text-xl text-slate-600 mb-8">Student ID: {studentId}</p>

        <div className="grid grid-cols-1 md:grid-cols-4 gap-6 mb-12">
          <div className="bg-white p-6 rounded-lg shadow-md">
            <p className="text-sm text-slate-600 mb-2">Total Exams</p>
            <p className="text-3xl font-bold">{totalExams}</p>
          </div>
          <div className="bg-white p-6 rounded-lg shadow-md">
            <p className="text-sm text-slate-600 mb-2">Average Score</p>
            <p className="text-3xl font-bold">{averageScore.toFixed(1)}%</p>
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

        {summaries.length === 0 ? (
          <div className="bg-slate-50 border-2 border-slate-200 rounded-lg p-8 text-center">
            <div className="text-4xl mb-4">📭</div>
            <h3 className="text-xl font-semibold mb-2">No exams found</h3>
            <p className="text-slate-600">
              You don't have any graded exams yet
            </p>
          </div>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            {summaries.map((summary) => (
              <ExamSummaryCard key={summary.examId} summary={summary} />
            ))}
          </div>
        )}
      </div>
    </div>
  );
}
