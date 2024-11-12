<?php

namespace Database\Seeders;

use App\Models\Account;
use App\Models\User;
use Illuminate\Database\Console\Seeds\WithoutModelEvents;
use Illuminate\Database\Seeder;
use Illuminate\Support\Facades\Hash;

class AccountsTableSeeder extends Seeder
{
    /**
     * Run the database seeds.
     */
    public function run(): void
    {
        //シーダーを使った初期データの登録
        Account::create([
            'name' => 'jobi',
            'password' => Hash::make('jobi') //LaravelのHashクラス
        ]);
    }
}
