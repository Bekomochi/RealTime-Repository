<?php

namespace Database\Seeders;

use App\Models\User;
use Illuminate\Database\Console\Seeds\WithoutModelEvents;
use Illuminate\Database\Seeder;

class UserTableSeeder extends Seeder
{
    /**
     * Run the database seeds.
     */
    public function run(): void
    {
        /*シーダーでファクトリーを呼び出す。
          数値は、生成するレコード数。*/
        //User::factory(20)->create();

        /* User::create([
             'name' => 'みうら',
             'password' => 'Fennekin'
         ]);*/
    }
}
