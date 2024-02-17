﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpusLink.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.Configurations
{
    public class UserAndRoleConfiguration : IEntityTypeConfiguration<UserAndRole>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<UserAndRole> builder)
        {
            builder.ToTable("UserAndRole");
            builder.HasData(
                new IdentityUserRole<int> { RoleId=1,UserId=9},
                new IdentityUserRole<int> { RoleId=3,UserId=1},
                new IdentityUserRole<int> { RoleId=3,UserId=2},
                new IdentityUserRole<int> { RoleId=3,UserId=3},
                new IdentityUserRole<int> { RoleId=2,UserId=4},
                new IdentityUserRole<int> { RoleId=2,UserId=5},
                new IdentityUserRole<int> { RoleId=2,UserId=6},
                new IdentityUserRole<int> { RoleId=2,UserId=7},
                new IdentityUserRole<int> { RoleId=2,UserId=8},

new IdentityUserRole<int> { RoleId = 3, UserId = 20 },
new IdentityUserRole<int> { RoleId = 3, UserId = 21 },
new IdentityUserRole<int> { RoleId = 3, UserId = 22 },
new IdentityUserRole<int> { RoleId = 3, UserId = 23 },
new IdentityUserRole<int> { RoleId = 3, UserId = 24 },
new IdentityUserRole<int> { RoleId = 3, UserId = 25 },
new IdentityUserRole<int> { RoleId = 3, UserId = 26 },
new IdentityUserRole<int> { RoleId = 3, UserId = 27 },
new IdentityUserRole<int> { RoleId = 3, UserId = 28 },
new IdentityUserRole<int> { RoleId = 3, UserId = 29 },
new IdentityUserRole<int> { RoleId = 3, UserId = 30 },
new IdentityUserRole<int> { RoleId = 3, UserId = 31 },
new IdentityUserRole<int> { RoleId = 3, UserId = 32 },
new IdentityUserRole<int> { RoleId = 3, UserId = 33 },
new IdentityUserRole<int> { RoleId = 3, UserId = 34 },
new IdentityUserRole<int> { RoleId = 3, UserId = 35 },
new IdentityUserRole<int> { RoleId = 3, UserId = 36 },
new IdentityUserRole<int> { RoleId = 3, UserId = 37 },
new IdentityUserRole<int> { RoleId = 3, UserId = 38 },
new IdentityUserRole<int> { RoleId = 3, UserId = 39 },
new IdentityUserRole<int> { RoleId = 3, UserId = 40 },
new IdentityUserRole<int> { RoleId = 3, UserId = 41 },
new IdentityUserRole<int> { RoleId = 3, UserId = 42 },
new IdentityUserRole<int> { RoleId = 3, UserId = 43 },
new IdentityUserRole<int> { RoleId = 3, UserId = 44 },
new IdentityUserRole<int> { RoleId = 3, UserId = 45 },
new IdentityUserRole<int> { RoleId = 3, UserId = 46 },
new IdentityUserRole<int> { RoleId = 3, UserId = 47 },
new IdentityUserRole<int> { RoleId = 3, UserId = 48 },
new IdentityUserRole<int> { RoleId = 3, UserId = 49 },
new IdentityUserRole<int> { RoleId = 3, UserId = 50 },
new IdentityUserRole<int> { RoleId = 3, UserId = 51 },
new IdentityUserRole<int> { RoleId = 3, UserId = 52 },
new IdentityUserRole<int> { RoleId = 3, UserId = 53 },
new IdentityUserRole<int> { RoleId = 3, UserId = 54 },
new IdentityUserRole<int> { RoleId = 3, UserId = 55 },
new IdentityUserRole<int> { RoleId = 3, UserId = 56 },
new IdentityUserRole<int> { RoleId = 3, UserId = 57 },
new IdentityUserRole<int> { RoleId = 3, UserId = 58 },
new IdentityUserRole<int> { RoleId = 3, UserId = 59 },
new IdentityUserRole<int> { RoleId = 3, UserId = 60 },
new IdentityUserRole<int> { RoleId = 3, UserId = 61 },
new IdentityUserRole<int> { RoleId = 3, UserId = 62 },
new IdentityUserRole<int> { RoleId = 3, UserId = 63 },
new IdentityUserRole<int> { RoleId = 3, UserId = 64 },
new IdentityUserRole<int> { RoleId = 3, UserId = 65 },
new IdentityUserRole<int> { RoleId = 3, UserId = 66 },
new IdentityUserRole<int> { RoleId = 3, UserId = 67 },
new IdentityUserRole<int> { RoleId = 3, UserId = 68 },
new IdentityUserRole<int> { RoleId = 3, UserId = 69 },
new IdentityUserRole<int> { RoleId = 3, UserId = 70 },
new IdentityUserRole<int> { RoleId = 3, UserId = 71 },
new IdentityUserRole<int> { RoleId = 3, UserId = 72 },
new IdentityUserRole<int> { RoleId = 3, UserId = 73 },
new IdentityUserRole<int> { RoleId = 3, UserId = 74 },
new IdentityUserRole<int> { RoleId = 3, UserId = 75 },
new IdentityUserRole<int> { RoleId = 3, UserId = 76 },
new IdentityUserRole<int> { RoleId = 3, UserId = 77 },
new IdentityUserRole<int> { RoleId = 3, UserId = 78 },
new IdentityUserRole<int> { RoleId = 3, UserId = 79 },
new IdentityUserRole<int> { RoleId = 3, UserId = 80 },
new IdentityUserRole<int> { RoleId = 3, UserId = 81 },
new IdentityUserRole<int> { RoleId = 3, UserId = 82 },
new IdentityUserRole<int> { RoleId = 3, UserId = 83 },
new IdentityUserRole<int> { RoleId = 3, UserId = 84 },
new IdentityUserRole<int> { RoleId = 3, UserId = 85 },
new IdentityUserRole<int> { RoleId = 3, UserId = 86 },
new IdentityUserRole<int> { RoleId = 3, UserId = 87 },
new IdentityUserRole<int> { RoleId = 3, UserId = 88 },
new IdentityUserRole<int> { RoleId = 3, UserId = 89 },
new IdentityUserRole<int> { RoleId = 3, UserId = 90 },
new IdentityUserRole<int> { RoleId = 3, UserId = 91 },
new IdentityUserRole<int> { RoleId = 3, UserId = 92 },
new IdentityUserRole<int> { RoleId = 3, UserId = 93 },
new IdentityUserRole<int> { RoleId = 3, UserId = 94 },
new IdentityUserRole<int> { RoleId = 3, UserId = 95 },
new IdentityUserRole<int> { RoleId = 3, UserId = 96 },
new IdentityUserRole<int> { RoleId = 3, UserId = 97 },
new IdentityUserRole<int> { RoleId = 3, UserId = 98 },
new IdentityUserRole<int> { RoleId = 3, UserId = 99 },
new IdentityUserRole<int> { RoleId = 3, UserId = 100 },




new IdentityUserRole<int> { RoleId = 2, UserId = 101 },
new IdentityUserRole<int> { RoleId = 2, UserId = 102 },
new IdentityUserRole<int> { RoleId = 2, UserId = 103 },
new IdentityUserRole<int> { RoleId = 2, UserId = 104 },
new IdentityUserRole<int> { RoleId = 2, UserId = 105 },
new IdentityUserRole<int> { RoleId = 2, UserId = 106 },
new IdentityUserRole<int> { RoleId = 2, UserId = 107 },
new IdentityUserRole<int> { RoleId = 2, UserId = 108 },
new IdentityUserRole<int> { RoleId = 2, UserId = 109 },
new IdentityUserRole<int> { RoleId = 2, UserId = 110 },
new IdentityUserRole<int> { RoleId = 2, UserId = 111 },
new IdentityUserRole<int> { RoleId = 2, UserId = 112 },
new IdentityUserRole<int> { RoleId = 2, UserId = 113 },
new IdentityUserRole<int> { RoleId = 2, UserId = 114 },
new IdentityUserRole<int> { RoleId = 2, UserId = 115 },
new IdentityUserRole<int> { RoleId = 2, UserId = 116 },
new IdentityUserRole<int> { RoleId = 2, UserId = 117 },
new IdentityUserRole<int> { RoleId = 2, UserId = 118 },
new IdentityUserRole<int> { RoleId = 2, UserId = 119 },
new IdentityUserRole<int> { RoleId = 2, UserId = 120 },
new IdentityUserRole<int> { RoleId = 2, UserId = 121 },
new IdentityUserRole<int> { RoleId = 2, UserId = 122 },
new IdentityUserRole<int> { RoleId = 2, UserId = 123 },
new IdentityUserRole<int> { RoleId = 2, UserId = 124 },
new IdentityUserRole<int> { RoleId = 2, UserId = 125 },
new IdentityUserRole<int> { RoleId = 2, UserId = 126 },
new IdentityUserRole<int> { RoleId = 2, UserId = 127 },
new IdentityUserRole<int> { RoleId = 2, UserId = 128 },
new IdentityUserRole<int> { RoleId = 2, UserId = 129 },
new IdentityUserRole<int> { RoleId = 2, UserId = 130 },
new IdentityUserRole<int> { RoleId = 2, UserId = 131 },
new IdentityUserRole<int> { RoleId = 2, UserId = 132 },
new IdentityUserRole<int> { RoleId = 2, UserId = 133 },
new IdentityUserRole<int> { RoleId = 2, UserId = 134 },
new IdentityUserRole<int> { RoleId = 2, UserId = 135 },
new IdentityUserRole<int> { RoleId = 2, UserId = 136 },
new IdentityUserRole<int> { RoleId = 2, UserId = 137 },
new IdentityUserRole<int> { RoleId = 2, UserId = 138 },
new IdentityUserRole<int> { RoleId = 2, UserId = 139 },
new IdentityUserRole<int> { RoleId = 2, UserId = 140 },
new IdentityUserRole<int> { RoleId = 2, UserId = 141 },
new IdentityUserRole<int> { RoleId = 2, UserId = 142 },
new IdentityUserRole<int> { RoleId = 2, UserId = 143 },
new IdentityUserRole<int> { RoleId = 2, UserId = 144 },
new IdentityUserRole<int> { RoleId = 2, UserId = 145 },
new IdentityUserRole<int> { RoleId = 2, UserId = 146 },
new IdentityUserRole<int> { RoleId = 2, UserId = 147 },
new IdentityUserRole<int> { RoleId = 2, UserId = 148 },
new IdentityUserRole<int> { RoleId = 2, UserId = 149 },
new IdentityUserRole<int> { RoleId = 2, UserId = 150 },
new IdentityUserRole<int> { RoleId = 2, UserId = 151 },
new IdentityUserRole<int> { RoleId = 2, UserId = 152 },
new IdentityUserRole<int> { RoleId = 2, UserId = 153 },
new IdentityUserRole<int> { RoleId = 2, UserId = 154 },
new IdentityUserRole<int> { RoleId = 2, UserId = 155 },
new IdentityUserRole<int> { RoleId = 2, UserId = 156 },
new IdentityUserRole<int> { RoleId = 2, UserId = 157 },
new IdentityUserRole<int> { RoleId = 2, UserId = 158 },
new IdentityUserRole<int> { RoleId = 2, UserId = 159 },
new IdentityUserRole<int> { RoleId = 2, UserId = 160 },
new IdentityUserRole<int> { RoleId = 2, UserId = 161 },
new IdentityUserRole<int> { RoleId = 2, UserId = 162 },
new IdentityUserRole<int> { RoleId = 2, UserId = 163 },
new IdentityUserRole<int> { RoleId = 2, UserId = 164 },
new IdentityUserRole<int> { RoleId = 2, UserId = 165 },
new IdentityUserRole<int> { RoleId = 2, UserId = 166 },
new IdentityUserRole<int> { RoleId = 2, UserId = 167 },
new IdentityUserRole<int> { RoleId = 2, UserId = 168 },
new IdentityUserRole<int> { RoleId = 2, UserId = 169 },
new IdentityUserRole<int> { RoleId = 2, UserId = 170 },
new IdentityUserRole<int> { RoleId = 2, UserId = 171 },
new IdentityUserRole<int> { RoleId = 2, UserId = 172 },
new IdentityUserRole<int> { RoleId = 2, UserId = 173 },
new IdentityUserRole<int> { RoleId = 2, UserId = 174 },
new IdentityUserRole<int> { RoleId = 2, UserId = 175 },
new IdentityUserRole<int> { RoleId = 2, UserId = 176 },
new IdentityUserRole<int> { RoleId = 2, UserId = 177 },
new IdentityUserRole<int> { RoleId = 2, UserId = 178 },
new IdentityUserRole<int> { RoleId = 2, UserId = 179 },
new IdentityUserRole<int> { RoleId = 2, UserId = 180 },
new IdentityUserRole<int> { RoleId = 2, UserId = 181 },
new IdentityUserRole<int> { RoleId = 2, UserId = 182 },
new IdentityUserRole<int> { RoleId = 2, UserId = 183 },
new IdentityUserRole<int> { RoleId = 2, UserId = 184 },
new IdentityUserRole<int> { RoleId = 2, UserId = 185 },
new IdentityUserRole<int> { RoleId = 2, UserId = 186 },
new IdentityUserRole<int> { RoleId = 2, UserId = 187 },
new IdentityUserRole<int> { RoleId = 2, UserId = 188 },
new IdentityUserRole<int> { RoleId = 2, UserId = 189 },
new IdentityUserRole<int> { RoleId = 2, UserId = 190 },
new IdentityUserRole<int> { RoleId = 2, UserId = 191 },
new IdentityUserRole<int> { RoleId = 2, UserId = 192 },
new IdentityUserRole<int> { RoleId = 2, UserId = 193 },
new IdentityUserRole<int> { RoleId = 2, UserId = 194 },
new IdentityUserRole<int> { RoleId = 2, UserId = 195 },
new IdentityUserRole<int> { RoleId = 2, UserId = 196 },
new IdentityUserRole<int> { RoleId = 2, UserId = 197 },
new IdentityUserRole<int> { RoleId = 2, UserId = 198 },
new IdentityUserRole<int> { RoleId = 2, UserId = 199 },
new IdentityUserRole<int> { RoleId = 2, UserId = 200 }

            );
        }
    }
}
