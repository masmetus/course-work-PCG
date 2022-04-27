﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;

namespace courswork
{
    class Explosion
    {
        // позиция взрыва
        private float[] position = new float[3];
        // можность
        private float _power;
        // максимальное количество частиц
        private int MAX_PARTICLES = 1000;
        // текущее установленное количество частиц
        private int _particles_now;

        // активирован
        private bool isStart = false;

        // массив частиц на основе созданного ранее класса
        private Partilce[] PartilceArray;

        // дисплейный список для рисования чатицы создан
        private bool isDisplayList = false;
        // номер дисплейного списка для отрисовки
        private int DisplayListNom = 0;

        // конструктор класса, в него передаются координаты, где должен произойти взрыв, можность и количество чатиц
        public Explosion(float x, float y, float z, float power, int particle_count)
        {
            position[0] = x;
            position[1] = y;
            position[2] = z;

            _particles_now = particle_count;
            _power = power;

            // если число частиц превышает максимально разрешенное
            if (particle_count > MAX_PARTICLES)
            {
                particle_count = MAX_PARTICLES;
            }

            // создаем массив частиц необходимого размера
            PartilceArray = new Partilce[particle_count];


        }

        // функция обновления позиции взрыва
        public void SetNewPosition(float x, float y, float z)
        {
            position[0] = x;
            position[1] = y;
            position[2] = z;
        }

        // установка нового значения мощности взрыва
        public void SetNewPower(float new_power)
        {
            _power = new_power;
        }

        // создания дисплейного списка для отрисовки частицы (т.к. отрисовывать даже небольшой полигон такое количество раз - очень накладно)
        private void CreateDisplayList()
        {
            // генерация дисплейног осписка
            DisplayListNom = Gl.glGenLists(1);

            // начало создания списка
            Gl.glNewList(DisplayListNom, Gl.GL_COMPILE);

            // редим отрисовки треугольника
            Gl.glBegin(Gl.GL_TRIANGLES);

            // задаем форму частицы
            Gl.glVertex3d(0, 0, 0);
            Gl.glVertex3d(0.02f, 0.02f, 0);
            Gl.glVertex3d(0.02f, 0, -0.02f);

            Gl.glEnd();

            // завершаем отрисовку частицы
            Gl.glEndList();

            // флаг - дисплейный список создан
            isDisplayList = true;
        }

        // функция реализующая взрыв
        public void Boooom(float time_start, float size, float lifeTime)
        {
            // инициализируем экземпляр класса Random
            Random rnd = new Random();

            // если дисплейный список не создан - надо его создать
            if (!isDisplayList)
            {
                CreateDisplayList();
            }

            // по всем частицам
            for (int ax = 0; ax < _particles_now; ax++)
            {
                // создаем частицу
                PartilceArray[ax] = new Partilce(position[0], position[1], position[2], size, lifeTime, time_start);

                // случайным образом генериуем ориентацию вектора ускорения для данной частицы
                int direction_x = rnd.Next(1, 3);
                int direction_y = rnd.Next(1, 3);
                int direction_z = rnd.Next(1, 3);

                // если сгенерированно число 2 - то мы заменяем его на -1. 
                if (direction_x == 2)
                    direction_x = -1;


                if (direction_y == 2)
                    direction_y = -1;


                if (direction_z == 2)
                    direction_z = -1;

                // задаем мощность в промежутке от 5 до 100% от указанной (чтобы частицы имели разное ускорение)
                float _power_rnd = rnd.Next((int)_power / 20, (int)_power);
                // устанавливаем затухание, равное 50% от мощности
                PartilceArray[ax].setAttenuation(_power / 2.0f);
                // устанавливаем ускорение частицы, еще раз генерируя случайное число. таким образом можность определится от 10 - до 100% полученной. Здесь же применяем ориентацию для векторов ускорения
                PartilceArray[ax].SetPower(_power_rnd * ((float)rnd.Next(100, 1000) / 1000.0f) * direction_x, _power_rnd * ((float)rnd.Next(100, 1000) / 1000.0f) * direction_y, _power_rnd * ((float)rnd.Next(100, 1000) / 1000.0f) * direction_z);
            }

            // взрыв активирован
            isStart = true;
        }

        // калькуляция текущего взрыва
        public void Calculate(float time)
        {
            // только в том случае, если взрыв уже активирован
            if (isStart)
            {
                // проходим циклом по всем частицам
                Random random = new Random();
                for (int ax = 0; ax < _particles_now; ax++)
                {
                    // если время жизни частицы еще не вышло
                    if (PartilceArray[ax].isLife())
                    {
                        // обновляем позицию частицы
                        PartilceArray[ax].UpdatePosition(time);

                        // сохраняем текущую матрицу
                        Gl.glPushMatrix();
                        // получаем размер частицы
                        float size = PartilceArray[ax].GetSize();

                        // выполняем перемещение частицы в необходимую позицию
                        //Gl.glColor3b(255,237,0);
                        //Gl.glColor3b(135, 206, 235);
                        if (random.Next(2) == 0)
                        {
                            Gl.glColor3f(1, 0.92f, 0);
                        }
                        else 
                        {
                            Gl.glColor3f(0.52f, 0.80f, 1);
                        }
                        Gl.glTranslated(PartilceArray[ax].GetPositionX(), PartilceArray[ax].GetPositionY(), PartilceArray[ax].GetPositionZ());
                        // масштабируем ее в соотвествии с ее размером
                        Gl.glScalef(size, size, size);
                        // вызываем дисплейный список для отрисовки частицы из кеша видеоадаптера
                        Gl.glCallList(DisplayListNom);
                        // возвращаем матрицу
                        Gl.glPopMatrix();

                        // отражение от "земли"
                        // если координата Y стала меньше нуля (удар о землю)
                        if (PartilceArray[ax].GetPositionY() < 0)
                        {
                            // инвертируем проекцию скорости на ось Y, как будто частица ударилась и отскочила от земли
                            // причем скорость затухает на 40%
                            PartilceArray[ax].InvertSpeed(1, 0.6f);
                        }
                    }

                }
            }
        }

    }
}
