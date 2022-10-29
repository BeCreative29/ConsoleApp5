public class GameField , IDisposable
{

        //поля
        public event FieldEventHandler VictoryEvent;
public delegate void FieldEventHandler(object sender, string message);
private Button[] innerbuttons = null;
private Size fieldsize = new Size(4, 4);         // размер поля
private int sidelength = 50;                    // размер стороны кнопки
private Point location = new Point(12, 41);     // позиция поля на форме
private int betweenbuttondist = 6;              // расстояние между кнопками на форме
private Form myform = null;                     // форма, на которой расположено поле

//конструкторы, при желании сюда можно еще что-то добавить:)
public GameField()
{
    FillButtons();
}

public GameField(Form form)
{
    if (form == null) { throw new ArgumentException("Параметр не может быть равен null!"); }
    FillButtons();
    foreach (Button b in innerbuttons) { form.Controls.Add(b); }
}

public GameField(Form form, Button[] Buttons)
{
    if (form == null || Buttons == null) { throw new ArgumentException("Параметр не может быть равен null!"); }
    if (Buttons.Length != FieldElementsCount) { throw new ArgumentException("Количество кнопок должно соответствовать размеру поля!"); }
    for (int i = 0; i < FieldElementsCount; i++) { if (Buttons[i] == null || Buttons[i].Width != Buttons[i].Height || Buttons[i].Size != Buttons[0].Size) { throw new ArgumentException("Неподходящие кнопки!"); } }
    this.sidelength = Buttons[0].Width;
    this.innerbuttons = Buttons;
    FillButtons();
    foreach (Button b in innerbuttons) { form.Controls.Add(b); }
}

public GameField(Form form, Button[] Buttons, Point location, int betweenbuttondist, Size fieldsize)
{
    if (form == null || Buttons == null) { throw new ArgumentException("Параметр не может быть равен null!"); }
    if (fieldsize.Width <= 0 || fieldsize.Height <= 0) { throw new ArgumentException("Размер поля не может быть нулевым!"); }
    if (location.X < 0 || location.Y < 0) { throw new ArgumentException("Позиция поля не может быть отрицательной"); }
    this.fieldsize = fieldsize;
    if (Buttons.Length != FieldElementsCount) { throw new ArgumentException("Количество кнопок должно соответствовать размеру поля!"); }
    for (int i = 0; i < FieldElementsCount; i++) { if (Buttons[i] == null || Buttons[i].Width != Buttons[i].Height || Buttons[i].Size != Buttons[0].Size) { throw new ArgumentException("Неподходящие кнопки!"); } }
    this.betweenbuttondist = betweenbuttondist;
    this.sidelength = Buttons[0].Width;
    this.innerbuttons = Buttons;
    this.location = location;
    FillButtons();
    foreach (Button b in innerbuttons) { form.Controls.Add(b); }
}


//свойства
public Form Form
{
    get
    {
        return myform;
    }
    set
    {
        if (value == null) { throw new ArgumentException("Параметр не может быть равен null"); }
        foreach (Button b in innerbuttons) { if (myform != null) { myform.Controls.Remove(b); } value.Controls.Add(b); }
        myform = value;
    }
}

private int LastButtonNumber
{
    get { return FieldElementsCount - 1; }
}
private int FieldElementsCount
{
    get { return fieldsize.Width * fieldsize.Height; }
}

//методы
private void FillButtons()
{
    if (innerbuttons == null) { innerbuttons = new Button[FieldElementsCount]; }
    for (int i = 0; i < FieldElementsCount; i++)
    {
        if (innerbuttons[i] == null)
        {
            innerbuttons[i] = new Button();
            innerbuttons[i].Size = new Size(sidelength, sidelength);
            innerbuttons[i].Text = (i + 1).ToString();
        }
        SetPosition(new Point(i % fieldsize.Width, i / fieldsize.Width), innerbuttons[i]);
        innerbuttons[i].Click += new EventHandler(Buttons_Click);
    }
    innerbuttons[LastButtonNumber].Visible = false;
}

private Point GetPosition(Button b)
{
    return new Point((b.Location.X - location.X) / (sidelength + betweenbuttondist), (b.Location.Y - location.Y) / (sidelength + betweenbuttondist));
}

private void SetPosition(Point pos, Button b)
{
    b.Location = new Point(location.X + pos.X * (sidelength + betweenbuttondist), location.Y + pos.Y * (sidelength + betweenbuttondist));
}

private bool victory()
{
    for (int i = 0; i < FieldElementsCount; i++)
    {
        if (GetPosition(innerbuttons[i]) != new Point(i % fieldsize.Width, i / fieldsize.Width))
        {
            return false;
        }
    }
    return true;
}

private void Buttons_Click(object sender, EventArgs e)
{
    Button Now = (Button)sender;
    int x = Math.Abs(GetPosition(Now).X - GetPosition(innerbuttons[LastButtonNumber]).X);
    int y = Math.Abs(GetPosition(Now).Y - GetPosition(innerbuttons[LastButtonNumber]).Y);
    if ((x == 1 && y == 0) || (x == 0 && y == 1))
    {
        Point P = GetPosition(Now);
        SetPosition(GetPosition(innerbuttons[LastButtonNumber]), Now);
        SetPosition(P, innerbuttons[LastButtonNumber]);
        if (victory()) { VictoryEvent(this, "Вы победили!"); }
    }
}

public void NewGame()
{
    Point[] arr = new Point[FieldElementsCount];
    for (int i = 0; i < arr.Length; i++) { arr[i] = new Point(i % fieldsize.Width, i / fieldsize.Width); }
    Random rand = new Random();
    arr = arr.OrderBy(c => rand.NextDouble()).ToArray();
    for (int i = 0; i < fieldsize.Width * fieldsize.Height; i++) { SetPosition(arr[i], innerbuttons[i]); }
}
public void Dispose()
{
    foreach (Button b in innerbuttons) { b.Dispose(); }
    myform = null;
}
 
    }
Пример на форме:
           
