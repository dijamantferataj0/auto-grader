'use client';

import Link from 'next/link';
import { ExamSummary } from '@/lib/types';

interface ResultsTableProps {
  summaries: ExamSummary[];
}

export default function ResultsTable({ summaries }: ResultsTableProps) {
  const getScoreColor = (percentage: number) => {
    if (percentage >= 80) return 'text-green-600';
    if (percentage >= 50) return 'text-yellow-600';
    return 'text-red-600';
  };

  const getScoreBadge = (percentage: number) => {
    if (percentage >= 80) return 'bg-green-100 text-green-800';
    if (percentage >= 50) return 'bg-yellow-100 text-yellow-800';
    return 'bg-red-100 text-red-800';
  };

  return (
    <div className="w-full overflow-x-auto">
      <table className="w-full border-collapse bg-white shadow-md rounded-lg overflow-hidden">
        <thead className="bg-slate-100">
          <tr>
            <th className="px-6 py-3 text-left text-sm font-semibold text-slate-700">Student ID</th>
            <th className="px-6 py-3 text-left text-sm font-semibold text-slate-700">Exam ID</th>
            <th className="px-6 py-3 text-left text-sm font-semibold text-slate-700">Total Tasks</th>
            <th className="px-6 py-3 text-left text-sm font-semibold text-slate-700">Correct</th>
            <th className="px-6 py-3 text-left text-sm font-semibold text-slate-700">Score</th>
            <th className="px-6 py-3 text-left text-sm font-semibold text-slate-700">Actions</th>
          </tr>
        </thead>
        <tbody>
          {summaries.map((summary, index) => (
            <tr key={index} className="border-t hover:bg-slate-50">
              <td className="px-6 py-4 text-sm">{summary.studentId}</td>
              <td className="px-6 py-4 text-sm">{summary.examId}</td>
              <td className="px-6 py-4 text-sm">{summary.totalTasks}</td>
              <td className="px-6 py-4 text-sm">{summary.correctTasks}</td>
              <td className="px-6 py-4">
                <span className={`inline-block px-3 py-1 rounded-full text-sm font-semibold ${getScoreBadge(summary.scorePercentage)}`}>
                  {summary.scorePercentage.toFixed(1)}%
                </span>
              </td>
              <td className="px-6 py-4">
                <Link
                  href={`/analytics/${summary.examId}`}
                  className="text-blue-600 hover:text-blue-800 text-sm font-medium"
                >
                  View Details →
                </Link>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
