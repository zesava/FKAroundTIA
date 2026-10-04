using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FKAroundTIA.Services
{
    public class GridClipboardService
    {
        public void CopySelectedCells(DataGridView grid)
        {
            if (grid == null) throw new ArgumentNullException(nameof(grid));

            var cells = GetCellsToCopy(grid);
            if (cells.Count == 0) return;

            var rowIndexes = cells.Select(cell => cell.RowIndex).Distinct().OrderBy(index => index).ToList();
            var columns = cells
                .Select(cell => grid.Columns[cell.ColumnIndex])
                .Distinct()
                .Where(column => column.Visible)
                .OrderBy(column => column.DisplayIndex)
                .ToList();

            var selectedKeys = new HashSet<string>(cells.Select(cell => GetCellKey(cell.RowIndex, cell.ColumnIndex)));
            var builder = new StringBuilder();

            for (int rowPosition = 0; rowPosition < rowIndexes.Count; rowPosition++)
            {
                if (rowPosition > 0)
                {
                    builder.AppendLine();
                }

                int rowIndex = rowIndexes[rowPosition];
                for (int columnPosition = 0; columnPosition < columns.Count; columnPosition++)
                {
                    if (columnPosition > 0)
                    {
                        builder.Append('\t');
                    }

                    var column = columns[columnPosition];
                    if (!selectedKeys.Contains(GetCellKey(rowIndex, column.Index)))
                    {
                        continue;
                    }

                    object value = grid.Rows[rowIndex].Cells[column.Index].Value;
                    builder.Append(FormatClipboardValue(value?.ToString() ?? ""));
                }
            }

            Clipboard.SetText(builder.ToString(), TextDataFormat.UnicodeText);
        }

        public int PasteClipboardValues(DataGridView grid, string readOnlyColumnName)
        {
            if (grid == null) throw new ArgumentNullException(nameof(grid));
            if (!Clipboard.ContainsText()) return 0;

            string text = Clipboard.GetText(TextDataFormat.UnicodeText);
            if (string.IsNullOrEmpty(text)) return 0;

            var values = ParseClipboardText(text);
            if (values.Count == 0) return 0;

            var startCell = GetPasteStartCell(grid);
            if (startCell == null) return 0;

            grid.EndEdit();

            int changedCells = 0;
            for (int rowOffset = 0; rowOffset < values.Count; rowOffset++)
            {
                int targetRowIndex = startCell.RowIndex + rowOffset;
                if (targetRowIndex >= grid.Rows.Count)
                {
                    break;
                }

                if (grid.Rows[targetRowIndex].IsNewRow)
                {
                    continue;
                }

                for (int columnOffset = 0; columnOffset < values[rowOffset].Count; columnOffset++)
                {
                    int targetColumnIndex = startCell.ColumnIndex + columnOffset;
                    if (targetColumnIndex >= grid.Columns.Count)
                    {
                        break;
                    }

                    var column = grid.Columns[targetColumnIndex];
                    if (!CanPasteToColumn(column, readOnlyColumnName))
                    {
                        continue;
                    }

                    grid.Rows[targetRowIndex].Cells[targetColumnIndex].Value = values[rowOffset][columnOffset];
                    changedCells++;
                }
            }

            return changedCells;
        }

        private static List<DataGridViewCell> GetCellsToCopy(DataGridView grid)
        {
            var cells = grid.SelectedCells
                .Cast<DataGridViewCell>()
                .Where(cell => cell.RowIndex >= 0 && cell.ColumnIndex >= 0 && cell.Visible)
                .ToList();

            if (cells.Count == 0 && grid.CurrentCell != null)
            {
                cells.Add(grid.CurrentCell);
            }

            return cells;
        }

        private static DataGridViewCell GetPasteStartCell(DataGridView grid)
        {
            if (grid.CurrentCell != null)
            {
                return grid.CurrentCell;
            }

            return grid.SelectedCells
                .Cast<DataGridViewCell>()
                .Where(cell => cell.RowIndex >= 0 && cell.ColumnIndex >= 0)
                .OrderBy(cell => cell.RowIndex)
                .ThenBy(cell => grid.Columns[cell.ColumnIndex].DisplayIndex)
                .FirstOrDefault();
        }

        private static bool CanPasteToColumn(DataGridViewColumn column, string readOnlyColumnName)
        {
            if (column == null || !column.Visible || column.ReadOnly)
            {
                return false;
            }

            return !string.Equals(column.Name, readOnlyColumnName, StringComparison.OrdinalIgnoreCase)
                && !string.Equals(column.HeaderText, readOnlyColumnName, StringComparison.OrdinalIgnoreCase);
        }

        private static string FormatClipboardValue(string value)
        {
            if (value.IndexOfAny(new[] { '\t', '\r', '\n', '"' }) < 0)
            {
                return value;
            }

            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }

        private static List<List<string>> ParseClipboardText(string text)
        {
            var rows = new List<List<string>>();
            var row = new List<string>();
            var cell = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < text.Length; i++)
            {
                char current = text[i];

                if (inQuotes)
                {
                    if (current == '"')
                    {
                        if (i + 1 < text.Length && text[i + 1] == '"')
                        {
                            cell.Append('"');
                            i++;
                        }
                        else
                        {
                            inQuotes = false;
                        }
                    }
                    else
                    {
                        cell.Append(current);
                    }

                    continue;
                }

                if (current == '"')
                {
                    inQuotes = true;
                }
                else if (current == '\t')
                {
                    row.Add(cell.ToString());
                    cell.Clear();
                }
                else if (current == '\r' || current == '\n')
                {
                    row.Add(cell.ToString());
                    cell.Clear();
                    rows.Add(row);
                    row = new List<string>();

                    if (current == '\r' && i + 1 < text.Length && text[i + 1] == '\n')
                    {
                        i++;
                    }
                }
                else
                {
                    cell.Append(current);
                }
            }

            row.Add(cell.ToString());
            if (row.Count > 1 || row[0].Length > 0)
            {
                rows.Add(row);
            }

            return rows;
        }

        private static string GetCellKey(int rowIndex, int columnIndex)
        {
            return rowIndex + ":" + columnIndex;
        }
    }
}
