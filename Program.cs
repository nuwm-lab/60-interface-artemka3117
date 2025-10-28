using System;
using System.Collections.Generic;
using System.Globalization;

/// <summary>
/// Абстрактний клас для півплощини/півпростору
/// </summary>
public abstract class AbstractHalfObject
{
    public abstract void PrintCoefficients();
    public abstract bool ContainsPoint(double[] coords);
    public abstract void SetCoefficients(params double[] coeffs);
    public abstract override string ToString();
    // Конструктор
    public AbstractHalfObject() { }
    // Деструктор
    ~AbstractHalfObject() { }
}

/// <summary>
/// Описує півплощину у 2D.
/// </summary>
public class HalfPlane : AbstractHalfObject
{
    private double _a1, _a2, _b;
    public double A1 => _a1;
    public double A2 => _a2;
    public double B => _b;

    public HalfPlane(double a1, double a2, double b)
    {
        _a1 = a1;
        _a2 = a2;
        _b = b;
    }
    // Деструктор
    ~HalfPlane() { }

    /// <summary>
    /// Встановлює коефіцієнти півплощини
    /// </summary>
    public override void SetCoefficients(params double[] coeffs)
    {
        if (coeffs.Length == 3)
        {
            _a1 = coeffs[0];
            _a2 = coeffs[1];
            _b = coeffs[2];
        }
    }

    /// <summary>
    /// Виводить коефіцієнти півплощини
    /// </summary>
    public override void PrintCoefficients()
    {
        Console.WriteLine($"Коефіцієнти півплощини: a1={_a1}, a2={_a2}, b={_b}");
    }

    /// <summary>
    /// Перевіряє, чи належить точка півплощині (2D)
    /// </summary>
    public override bool ContainsPoint(double[] coords)
    {
        if (coords == null || coords.Length != 2)
            return false;
        return _a1 * coords[0] + _a2 * coords[1] <= _b;
    }

    public override string ToString() => $"HalfPlane: a1={_a1}, a2={_a2}, b={_b}";
}

/// <summary>
/// Описує півпростір у 3D.
/// </summary>
public class HalfSpace : AbstractHalfObject
{
    private double _a1, _a2, _a3, _b;
    public double A1 => _a1;
    public double A2 => _a2;
    public double A3 => _a3;
    public double B => _b;

    public HalfSpace(double a1, double a2, double a3, double b)
    {
        _a1 = a1;
        _a2 = a2;
        _a3 = a3;
        _b = b;
    }
    // Деструктор
    ~HalfSpace() { }

    /// <summary>
    /// Встановлює коефіцієнти півпростору
    /// </summary>
    public override void SetCoefficients(params double[] coeffs)
    {
        if (coeffs.Length == 4)
        {
            _a1 = coeffs[0];
            _a2 = coeffs[1];
            _a3 = coeffs[2];
            _b = coeffs[3];
        }
    }

    /// <summary>
    /// Виводить коефіцієнти півпростору
    /// </summary>
    public override void PrintCoefficients()
    {
        Console.WriteLine($"Коефіцієнти півпростору: a1={_a1}, a2={_a2}, a3={_a3}, b={_b}");
    }

    /// <summary>
    /// Перевіряє, чи належить точка півпростору (3D)
    /// </summary>
    public override bool ContainsPoint(double[] coords)
    {
        if (coords == null || coords.Length != 3)
            return false;
        return _a1 * coords[0] + _a2 * coords[1] + _a3 * coords[2] <= _b;
    }

    public override string ToString() => $"HalfSpace: a1={_a1}, a2={_a2}, a3={_a3}, b={_b}";
}

class Program
{
    /// <summary>
    /// Зчитує масив чисел з консолі
    /// </summary>
    private static double[] ReadDoubles(string prompt, int count)
    {
        while (true)
        {
            Console.WriteLine(prompt);
            var input = Console.ReadLine();
            if (input == null)
            {
                Console.WriteLine("Ввід перервано.");
                return null;
            }
            var arr = input.Split();
            if (arr.Length != count)
            {
                Console.WriteLine($"Помилка: потрібно {count} чисел.");
                continue;
            }
            var result = new double[count];
            bool ok = true;
            for (int i = 0; i < count; i++)
                ok &= double.TryParse(arr[i], NumberStyles.Float, CultureInfo.InvariantCulture, out result[i]);
            if (ok) return result;
            Console.WriteLine("Помилка: некоректний ввід.");
        }
    }

    static void Main()
    {
        var objects = new List<AbstractHalfObject>();
        var points = new List<double[]>();
        while (true)
        {
            Console.WriteLine("Виберіть тип об'єкта: 1 - Півплощина, 2 - Півпростір, 0 - Вихід");
            var choice = Console.ReadLine();
            if (choice == "0") break;
            if (choice == "1")
            {
                var hpCoeffs = ReadDoubles("Введіть коефіцієнти для півплощини (a1 a2 b):", 3);
                if (hpCoeffs == null) continue;
                AbstractHalfObject obj = new HalfPlane(hpCoeffs[0], hpCoeffs[1], hpCoeffs[2]);
                objects.Add(obj);
                var hpPoint = ReadDoubles("Введіть точку для перевірки (x1 x2):", 2);
                points.Add(hpPoint);
            }
            else if (choice == "2")
            {
                var hsCoeffs = ReadDoubles("Введіть коефіцієнти для півпростору (a1 a2 a3 b):", 4);
                if (hsCoeffs == null) continue;
                AbstractHalfObject obj = new HalfSpace(hsCoeffs[0], hsCoeffs[1], hsCoeffs[2], hsCoeffs[3]);
                objects.Add(obj);
                var hsPoint = ReadDoubles("Введіть точку для перевірки (x1 x2 x3):", 3);
                points.Add(hsPoint);
            }
            else
            {
                Console.WriteLine("Некоректний вибір.");
            }
        }

        // Демонстрація встановлення коефіцієнтів для першого об'єкта кожного типу
        bool hpCoeffsChanged = false;
        bool hsCoeffsChanged = false;
        foreach (var obj in objects)
        {
            if (!hpCoeffsChanged && obj is HalfPlane hp && obj.GetType() == typeof(HalfPlane))
            {
                Console.WriteLine("\nВи можете змінити коефіцієнти для першої півплощини.");
                var newCoeffs = ReadDoubles("Введіть нові коефіцієнти (a1 a2 b):", 3);
                if (newCoeffs != null)
                    hp.SetCoefficients(newCoeffs);
                hpCoeffsChanged = true;
            }
            if (!hsCoeffsChanged && obj is HalfSpace hs)
            {
                Console.WriteLine("\nВи можете змінити коефіцієнти для першого півпростору.");
                var newCoeffs = ReadDoubles("Введіть нові коефіцієнти (a1 a2 a3 b):", 4);
                if (newCoeffs != null)
                    hs.SetCoefficients(newCoeffs);
                hsCoeffsChanged = true;
            }
        }

        Console.WriteLine("\nВсі створені об'єкти та перевірка точок:");
        for (int i = 0; i < objects.Count; i++)
        {
            var obj = objects[i];
            obj.PrintCoefficients();
            Console.WriteLine(obj);
            var pt = points[i];
            if (pt == null)
            {
                Console.WriteLine("Точка не задана.");
                continue;
            }
            bool result = obj.ContainsPoint(pt);
            Console.WriteLine(result ? "Точка належить об'єкту" : "Точка не належить об'єкту");
        }
    }
}
